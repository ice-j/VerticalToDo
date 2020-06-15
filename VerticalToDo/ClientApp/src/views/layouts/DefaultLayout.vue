<template>
  <div class="home">
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

    <v-navigation-drawer v-model="drawer" app clipped light class="elevation-1">
      <v-list dense nav>
        <v-list-item-group v-model="selectedIndex">
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
            <v-list-item v-else :key="i" :to="item.url" link>
              <v-list-item-icon>
                <v-icon>{{ item.icon }}</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title class="grey--text">
                  {{ item.text }}
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-list-item-group>
      </v-list>

      <template v-slot:append>
        <center class="pa-5">
          <v-btn href="https://github.com/ice-j" target="_blank" outlined>
            <v-icon left>mdi-github-circle</v-icon> GitHub
          </v-btn>
        </center>
      </template>
    </v-navigation-drawer>

    <v-main>
      <v-container fluid>
        <router-view></router-view>
      </v-container>
      <v-footer fixed>
        <v-row justify="center" no-gutters>
          &copy;{{ new Date().getFullYear() }}
        </v-row>
      </v-footer>
    </v-main>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component
export default class DefaultLayout extends Vue {
  name = "Vertical To Do";
  drawer: any = null;
  selectedIndex = 1;
  items = [
    { heading: "Tasks" },
    { icon: "list", text: "List", url: "/tasks/list" },
    { icon: "add", text: "Create new", url: "/tasks/new" },
    { divider: true },
    { icon: "archive", text: "Archive", url: "/tasks/archive" }
  ];
}
</script>
