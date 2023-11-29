<template>
  <tr>
    <td>
      {{ item.tag_id }}
    </td>

    <td v-if="!editing">
      {{ item.name }}
    </td>
    <td v-else>
      <input
        v-model="editedItem.name"
        class="form-control"
      >
    </td>

    <td v-if="!editing">
      {{ item.is_distractor }}
    </td>
    <td v-else>
      <div
        class="btn-group form-control p-0"
        role="group"
        aria-label="Is it a distractor"
      >
        <input
          id="idEditIsDistractor"
          type="radio"
          class="btn-check"
          name="btnradio"
          autocomplete="off"
          :checked="editedItem.is_distractor === true"
          @click="editedItem.is_distractor = true;"
        >
        <label
          class="btn btn-sm btn-outline-primary"
          for="idEditIsDistractor"
        >Yes</label>

        <input
          id="idEditNotDistractor"
          type="radio"
          class="btn-check"
          name="btnradio"
          autocomplete="off"
          :checked="editedItem.is_distractor === false"
          @click="editedItem.is_distractor = false;"
        >
        <label
          class="btn btn-sm btn-outline-primary"
          for="idEditNotDistractor"
        >No</label>
      </div>
    </td>

    <td>
      <button
        class="btn btn-sm btn-primary mx-2"
        type="button"
        @click="toggleEdit(false)"
      >
        {{ editing ? 'Save' : 'Edit' }}
      </button>
      <button
        v-if="editing"
        class="btn btn-sm btn-secondary mx-2"
        type="button"
        @click="toggleEdit(true)"
      >
        Cancel
      </button>
      <button
        v-if="!editing"
        class="btn btn-sm btn-danger mx-2"
        type="button"
        @click="deleteTag(item.tag_id)"
      >
        Delete
      </button>
    </td>
  </tr>
</template>
  
<script lang="ts">
import TagItem from "@/views/ManageTagView.vue";

export default {
    props: {
        item: {
            type: Object as () => TagItem,
            required: true,
        },
    },
    emits: ["save", "delete"],
    data() {
        return {
            editing: false,
            editedItem: { ...this.item } as TagItem,
        };
    },
    methods: {
        toggleEdit(cancel: boolean = false) {
            if (this.editing && !cancel) {
                this.$emit("save", this.editedItem);
            }
            this.editing = !this.editing;
        },
        deleteTag(tagId: string) {
            this.$emit("delete", tagId);
        }
    }
};
</script>
