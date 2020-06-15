<template>
  <v-row align="center" justify="center">
    <v-col cols="12" sm="8" md="4">
      <v-card class="elevation-12">
        <v-toolbar color="primary" dark flat>
          <v-toolbar-title>Login form</v-toolbar-title>
        </v-toolbar>
        <v-form on-submit="submit()" @submit.prevent="submit">
          <v-card-text>
            <v-text-field
              v-model="model.emailAddress"
              label="Email Address"
              prepend-icon="mdi-account"
              type="text"
            ></v-text-field>

            <v-text-field
              v-model="model.password"
              label="Password"
              prepend-icon="mdi-lock"
              type="password"
            ></v-text-field>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn type="submit" color="primary" class="ma-5">Login</v-btn>
          </v-card-actions>
        </v-form>
        <v-footer>
          Don't have an account yet?
          <v-btn text to="register" light>Register now</v-btn>
        </v-footer>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  AccountsApi,
  VerticalToDoServicesFeaturesAccountsLogInRequest,
} from "@/../swagger/api";
import { AuthenticationService } from "../../Services/AuthenticationService";

@Component
export default class Login extends Vue {
  model: VerticalToDoServicesFeaturesAccountsLogInRequest = {};
  async submit() {
    const res = await new AccountsApi().apiAccountsLogInPost(this.model);
    if (res?.status != 200 || !res.data.token) {
      return false;
    }

    AuthenticationService.logIn(res.data.token);
    const redirect = this.$route.params.redirectTo;
    if (redirect) this.$router.replace(redirect);
    else this.$router.replace("/tasks/list");
    return false;
  }
}
</script>
