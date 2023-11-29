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
      <i
        v-if="item.is_distractor"
        class="bi bi-x"
      />
      <i
        v-if="!item.is_distractor"
        class="bi bi-check"
      />
      {{ item.is_distractor ? "No" : "Yes" }}
    </td>
    <td v-else>
      <div
        class="btn-group form-control p-0"
        role="group"
        aria-label="Is it a distractor"
      >
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
        >
          <i class="bi bi-check" />Yes</label>
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
        >
          <i class="bi bi-x" />No</label>
      </div>
    </td>

    <td>
      <button
        class="btn btn-sm btn-primary mx-2"
        type="button"
        @click="toggleEdit(false)"
      >
        <i
          v-if="editing"
          class="bi bi-floppy"
        />
        <i
          v-if="!editing"
          class="bi bi-pencil-square"
        />
        {{ editing ? "Save" : "Edit" }}
      </button>
      <button
        v-if="editing"
        class="btn btn-sm btn-secondary mx-2"
        type="button"
        @click="toggleEdit(true)"
      >
        <i class="bi bi-ban" />
        Cancel
      </button>
      <button
        v-if="!editing"
        class="btn btn-sm btn-danger mx-2"
        type="button"
        @click="deleteTag(item.tag_id)"
      >
        <i class="bi bi-trash" />
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
