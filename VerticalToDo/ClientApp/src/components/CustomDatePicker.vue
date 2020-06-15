<template>
  <v-menu
    ref="menu"
    :close-on-content-click="false"
    :close-on-click="true"
    transition="scale-transition"
    offset-y
    max-width="290px"
    min-width="290px"
  >
    <template v-slot:activator="{ on }">
      <v-text-field
        v-model="selectedDate"
        :label="label"
        readonly
        clearable
        v-on="on"
      ></v-text-field>
    </template>
    <v-date-picker
      ref="dp"
      v-model="selectedDate"
      no-title
      :min="new Date().toISOString()"
    >
      <v-spacer></v-spacer>
      <a>Last 3 months</a>
      <v-btn text color="primary" @click="$refs.menu.save()">OK</v-btn>
    </v-date-picker>
  </v-menu>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

@Component
export default class CustomDatePicker extends Vue {
  @Prop() value!: Date | null;
  @Prop() label!: string;

  get selectedDate() {
    return this.value;
  }

  set selectedDate(value) {
    this.$emit("update:value", value);
  }
}
</script>
