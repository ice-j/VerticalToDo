<template>
  <v-card>
    <v-card-title>Create new To Do Item</v-card-title>
    <v-card-text>
      <v-form on-submit="submit()" @submit.prevent="submit">
        <v-text-field v-model="model.title" clearable label="Title" />
        <v-textarea
          v-model="model.description"
          clearable
          rows="10"
          label="Description"
        />
        <custom-date-picker :value.sync="model.dueDate" label="Due Date" />
        <v-card-actions>
          <v-btn color="primary" type="submit" large raised>Submit</v-btn>
          <v-btn text>Cancel</v-btn>
        </v-card-actions>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import CustomDatePicker from "@/components/CustomDatePicker.vue";
import {
  ToDosApi,
  VerticalToDoServicesFeaturesToDosNewRequest
} from "@/../swagger/api";

@Component({
  components: {
    CustomDatePicker
  }
})
export default class New extends Vue {
  model: VerticalToDoServicesFeaturesToDosNewRequest = {};
  async submit() {
    const res = await new ToDosApi().apiToDosNewPost(this.model);
    if (res?.status != 200) return;

    this.$router.push("/");
  }
}
</script>
