<template>
  <div>
    <BsNavbar />
    <div class="container">
      <div class="card m-2">
        <div class="card-body">
          <h1 class="card-title">
            Tag Manager
          </h1>
          <div class="card-text">
            <form>
              <div class="mb-3">
                <label
                  for="idNewTagId"
                  class="form-label"
                >Tag ID</label>
                <input
                  id="idNewTagId"
                  type="text"
                  class="form-control"
                  v-model="tag_id"
                  required
                >
              </div>
              <div class="mb-3">
                <label
                  for="idNewName"
                  class="form-label"
                >Tag Name</label>
                <input
                  id="idNewName"
                  type="text"
                  class="form-control"
                  v-model="name"
                  required
                >
              </div>
              <div class="mb-3 w-50">
                <label
                  for="idNewName"
                  class="form-label"
                >Is it distractor</label>
                <div
                  class="btn-group form-control"
                  role="group"
                  aria-label="Is it a distractor"
                >
                  <input
                    id="idNewIsDistractor"
                    type="radio"
                    class="btn-check"
                    name="btnradio"
                    autocomplete="off"
                    checked
                  >
                  <label
                    class="btn btn-outline-primary"
                    for="idNewIsDistractor"
                  >Yes</label>

                  <input
                    id="idNewNotDistractor"
                    type="radio"
                    class="btn-check"
                    name="btnradio"
                    autocomplete="off"
                  >
                  <label
                    class="btn btn-outline-primary"
                    for="idNewNotDistractor"
                  >No</label>
                </div>
              </div>

              <div class="mb-3">
                <button
                  type="button"
                  class="btn btn-primary"
                  @click="addTag()"
                >
                  Add
                </button>
              </div>
            </form>
            <hr>
            <table class="table table-bordered table-striped">
              <thead>
                <th>Tag Id</th>
                <th>Name</th>
                <th>Is Distractor</th>
                <th>Operations</th>
              </thead>
              <tbody>
                <tr
                  v-for="item in tagList"
                  :key="item.tag_id"
                >
                  <td>{{ item.tag_id }}</td>
                  <td>{{ item.name }}</td>
                  <td>{{ item.is_distractor }}</td>
                  <td>
                    <button class="btn btn-sm btn-primary mx-2">Edit</button>
                    <button class="btn btn-sm btn-danger mx-2" @click="del(item.tag_id)">Delete</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BsNavbar from "@/components/BsNavbar.vue";
import axios from "axios";

interface TagItem {
  tag_id: string;
  name: string;
  is_distractor: boolean;
}
export default {
  name: "ManageTagView",
  components: { BsNavbar },
  props: {
  },
  data() {
    return {
      tag_id: "",
      name: "",
      tagList: [] as TagItem[],
    };
  },
  mounted() {
        // Fetch tag IDs every second
        this.fetchTagIds();
    },
  methods: {
     fetchTagIds() {
        axios.post('/api/getAllTags').then((response) => {
          if (response.data.success) {
          // Assign the 'tags' to the data property
          this.tagList = response.data.tags;
          console.log(this.tagList);
        } else {
          console.error("API response indicates failure");
          // Handle the case where success is false
        }
        });   
    },
    addTag() {
      // Replace the URL with your backend API endpoint
      const backendURL = "/api/addTag";

      // Replace this with the data you want to send to the backend
      const dataToSend = {
        tag_id: this.tag_id,
        name: this.name,
        is_distractor: "true",
      };
      
      axios
        .post(backendURL, dataToSend)
        .then((response) => {
          console.log("Backend response:", response.data);
          if (response.data.success) {
          // Assign the 'tags' to the data property
          this.fetchTagIds();
        } else {
          console.error("API response indicates failure");
          // Handle the case where success is false
        }
        })
        .catch((error) => {
          console.error("Error sending data to the backend:", error);
        });
      }
    },

    del(tagId: string) {
      try {
        axios.delete(`/api/deleteTag/${tagId}`); // Replace with your backend API URL
        // Remove the deleted tag from the local tags array to update the UI
        this.tags = this.tags.filter(tag => tag.id !== tagId);
      } catch (error) {
        console.error('Error deleting tag:', error);
      }
    },
  };
</script>
