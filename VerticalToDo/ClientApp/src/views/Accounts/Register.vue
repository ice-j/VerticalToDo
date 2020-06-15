<template>
  <v-row align="center" justify="center">
    <v-col cols="12" sm="8" md="4">
      <v-card class="elevation-12">
        <v-toolbar color="primary" dark flat>
          <v-toolbar-title>Register form</v-toolbar-title>
        </v-toolbar>
        <v-card-text>
          <v-form>
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
            <v-text-field
              v-model="model.confirmPassword"
              label="Confirm Password"
              prepend-icon="mdi-lock"
              type="password"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" class="ma-5">Register</v-btn>
        </v-card-actions>
        <v-footer>
          Already have an account yet?
          <v-btn text to="login" light>Login now</v-btn>
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
  VerticalToDoServicesFeaturesAccountsRegisterRequest,
} from "@/../swagger/api";
import { AuthenticationService } from "../../Services/AuthenticationService";

@Component
export default class Register extends Vue {
  model: VerticalToDoServicesFeaturesAccountsRegisterRequest = {};
  async submit() {
    const res = await new AccountsApi().apiAccountsRegisterPost(this.model);
    if (res.status != 200 || !res.data.token) {
      return;
    }

    AuthenticationService.logIn(res.data.token);
    this.$router.replace("/");
  }
}
</script>
