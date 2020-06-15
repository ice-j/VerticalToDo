import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import "roboto-fontface/css/roboto/roboto-fontface.css";
import "@mdi/font/css/materialdesignicons.css";
import Axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from "axios";
import { AuthenticationService } from "./Services/AuthenticationService";
import VuetifyConfirm from "vuetify-confirm";
import VuetifyToast from "vuetify-toast-snackbar";

Vue.use(VuetifyToast, {
  x: "right", // default
  y: "bottom", // default
  color: "info", // default
  icon: "info",
  iconColor: "", // default
  classes: ["body-2"],
  timeout: 3000, // default
  dismissable: true, // default
  multiLine: false, // default
  vertical: false, // default
  queueable: false, // default
  showClose: false, // default
  closeText: "", // default
  closeIcon: "close", // default
  closeColor: "", // default
  slot: [], //default
  shorts: {
    custom: {
      color: "purple"
    }
  },
  property: "$toast" // default
});

Vue.config.productionTip = false;

Axios.defaults.validateStatus = () => true;
Axios.interceptors.request.use(
  (req: AxiosRequestConfig) => {
    req.headers["Authorization"] = AuthenticationService.getToken();
    return req;
  },
  req => {
    console.log(req);
  }
);
Axios.interceptors.response.use(
  (res: AxiosResponse<any>) => {
    if (res?.status == 401) {
      AuthenticationService.logOut();
      store.commit("authChanged");
      return res;
    }

    const errors = new Map<string, string>();
    let errorsString = "";
    //handle empty response
    if (res == null || res == undefined) {
      errors.set("Error", "Something went wrong. Please try again later.");
    }
    if (res.data.Error) {
      errors.set("Error", res.data.Error);
      store.commit("onError", res.data.Error);
    } else if (res.status >= 400 && res.status < 500) {
      res.data.Errors.forEach(item => {
        errors.set(item.PropertyName, item.ErrorMessage);
        errorsString += "<br />" + item.ErrorMessage;
      });
      errorsString = errorsString.substring(6);
      store.commit("errors", errorsString);
    } else if (res.status >= 500) {
      Object.keys(res.data).forEach(function (key) {
        errors.set(key, res.data[key]);
        errorsString += "<br />" + res.data[key];
      });
      errorsString = errorsString.substring(6);
      store.commit("errors", errorsString);
    }

    if (errors.size > 0) {
      errors.forEach(v => {
        // Vue.prototype.$notify.error(v);
      });
    }
    return res;
  },
  (error: AxiosError) => {
    if (error?.response?.status == 401) {
      AuthenticationService.logOut();
      store.commit("authChanged");
    }
    return Promise.reject(error);
  }
);

Vue.use(VuetifyConfirm, {
  vuetify,
  buttonTrueText: "Accept",
  buttonFalseText: "Discard",
  buttonTrueColor: "error",
  color: "error",
  icon: "warning",
  title: "Warning",
  width: 350,
  property: "$confirm"
});

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount("#app");
