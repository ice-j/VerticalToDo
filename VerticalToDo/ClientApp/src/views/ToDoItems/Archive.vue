<template>
  <v-container v-if="items" fluid>
    <v-data-iterator
      :items="items"
      :items-per-page.sync="itemsPerPage"
      :page="currentPage"
    >
      <template v-slot:default="props">
        <v-row>
          <v-col
            v-for="item in props.items"
            :key="item.id"
            cols="12"
            sm="6"
            md="4"
            lg="3"
          >
            <v-card>
              <v-card-title class="subheading font-weight-bold"
                >{{ item.title }}
              </v-card-title>
              <v-divider></v-divider>
              <v-card-text>{{ item.description }}</v-card-text>
              <v-card-actions v-if="item.dueDate">
                <v-icon>mdi-clock</v-icon>
                {{ item.dueDate }}
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </template>
    </v-data-iterator>
  </v-container>
  <v-card v-else>
    <v-card-title>Error</v-card-title>
    <v-card-text>{{ error }}</v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  ToDosApi,
  VerticalToDoServicesFeaturesToDosListResponseItem
} from "@/../swagger/api";

@Component
export default class Archive extends Vue {
  items: VerticalToDoServicesFeaturesToDosListResponseItem[] = [];
  error = "";
  itemsPerPage = 10;
  currentPage = 1;
  dialog = false;
  async mounted() {
    const res = await new ToDosApi().apiToDosListArchivedGet(
      this.currentPage,
      this.itemsPerPage
    );
    if (!res || res.status != 200 || !res.data.items) {
      this.error = res?.statusText;
      return;
    }
    this.items = res.data.items;
  }
}
</script>
