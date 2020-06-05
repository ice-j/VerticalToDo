<template>
  <v-app id="keep">
    <v-app-bar app clipped-left dark color="primary">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <span class="title ml-3 mr-5"
        >Vertical&nbsp;<span class="font-weight-light">To Do</span></span
      >
      <v-text-field
        solo-inverted
        flat
        hide-details
        label="Search"
        prepend-inner-icon="search"
      ></v-text-field>

      <v-spacer></v-spacer>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" app clipped light>
      <v-list dense>
        <template v-for="(item, i) in items">
          <v-row v-if="item.heading" :key="i" align="center">
            <v-col cols="6">
              <v-subheader v-if="item.heading">
                {{ item.heading }}
              </v-subheader>
            </v-col>
          </v-row>
          <v-divider
            v-else-if="item.divider"
            :key="i"
            dark
            class="my-4"
          ></v-divider>
          <v-list-item v-else :key="i" link>
            <v-list-item-action>
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title class="grey--text">
                {{ item.text }}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>

    <v-content>
      <router-view></router-view>
      <v-footer absolute dark color="primary">
        <v-row justify="center" no-gutters>
          &copy;{{ new Date().getFullYear() }}
        </v-row>
      </v-footer>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import { Component } from "vue-property-decorator";

@Component
export default class App extends Vue {
  name = "Vertical To Do";
  drawer: any = null;
  items = [
    { heading: "Tasks" },
    { icon: "add", text: "Create new" },
    { icon: "list", text: "List" },
    { divider: true },
    { icon: "archive", text: "Archive" },
    { icon: "delete", text: "Trash" }
  ];
}
</script>

<style scoped>
#keep .v-navigation-drawer__border {
  display: none;
}
</style>
