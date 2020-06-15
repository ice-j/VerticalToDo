<template>
  <v-main>
    <v-container class="fill-height" fluid>
      <router-view></router-view>
    </v-container>
  </v-main>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component
export default class AuthLayout extends Vue {}
</script>
