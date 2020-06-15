<template>
  <v-app id="keep">
    <router-view></router-view>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import { Component } from "vue-property-decorator";

@Component
export default class App extends Vue {}
</script>

<style scoped>
#keep .v-navigation-drawer__border {
  display: none;
}
</style>
