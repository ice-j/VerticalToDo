<template>
  <v-card>
    <v-card-title>
      <span v-if="model.title"> {{ "Edit " + model.title }}</span>
      <span v-else>Edit</span>
    </v-card-title>
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
  VerticalToDoServicesFeaturesToDosUpdateRequest
} from "@/../swagger/api";
import { Watch } from "vue-property-decorator";

@Component({
  components: {
    CustomDatePicker
  }
})
export default class Edit extends Vue {
  model: VerticalToDoServicesFeaturesToDosUpdateRequest = {};

  async mounted() {
    var id = this.$route.params.id;
    console.log(id);
    if (!id) {
      this.$toast.error("Item not found");
      return;
    }

    const res = await new ToDosApi().apiToDosDetailsGet(id);
    if (res.status != 200 || !res.data) return;

    this.model = {
      id: res.data.id,
      title: res.data.title,
      description: res.data.description,
      dueDate: res.data.dueDate
    };
    console.log(this.model);
  }

  async submit() {
    const res = await new ToDosApi().apiToDosUpdatePost(this.model);
    if (res?.status != 200) return;

    this.$router.push("/");
  }
}
</script>
