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
                <v-spacer />
                <v-btn icon :to="`edit/${item.id}`" title="Edit">
                  <v-icon>mdi-pen</v-icon>
                </v-btn>
                <v-btn
                  icon
                  color="error"
                  @click="remove(item.id)"
                  title="Delete"
                >
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </v-card-title>
              <v-divider></v-divider>
              <v-card-text>{{ item.description }}</v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </template>
    </v-data-iterator>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  ToDosApi,
  VerticalToDoServicesFeaturesToDosListResponseItem
} from "@/../swagger/api";

@Component
export default class List extends Vue {
  items: VerticalToDoServicesFeaturesToDosListResponseItem[] = [];
  itemsPerPage = 10;
  currentPage = 1;
  dialog = false;
  async mounted() {
    const res = await new ToDosApi().apiToDosListGet(
      this.currentPage,
      this.itemsPerPage
    );
    if (!res || res.status != 200 || !res.data.items) {
      this.$toast.error(res?.statusText);
      return;
    }
    this.items = res.data.items;
  }

  async remove(id: string) {
    const item = this.items.find(x => x.id == id);
    debugger;
    if (item == null) return;
    this.$confirm(`Are you sure you want to delete ${item.title}`).then(
      async res => {
        if (!res) return;
        var apiRes = await new ToDosApi().apiToDosArchivePost({ id: item.id });
        if (apiRes.status != 200) return;

        this.items = this.items.filter(x => x.id != item.id);
      }
    );
  }
}
</script>
