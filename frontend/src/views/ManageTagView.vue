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
                >Tag Id</label>
                <input
                  id="idNewTagId"
                  type="text"
                  class="form-control"
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
                  @click="add()"
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
                  v-for="item in list"
                  :key="item.id"
                >
                  <td>{{ item.id }}</td>
                  <td>{{ item.name }}</td>
                  <td>{{ item.isTrue }}</td>
                  <td>
                    <button class="btn btn-sm btn-primary mx-2">Edit</button>
                    <button class="btn btn-sm btn-danger mx-2" @click="del(item.id)">Delete</button>
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

interface ListItem {
  id: string;
  name: string;
  isTrue: boolean;
}
export default {
  name: "ManageTagView",
  components: { BsNavbar },
  props: {
    msg: String,
  },
  data() {
    return {
      id: "",
      name: "",
      keywords: "",
      list: [
        { id: "1", name: "paper", isTrue: true },
        { id: "2", name: "bottle", isTrue: false },
        { id: "3", name: "mouse", isTrue: false },
        { id: "4", name: "pen", isTrue: true },
        { id: "5", name: "computer", isTrue: true },
      ] as ListItem[],
    };
  },
  methods: {
    sendData() {
      // Replace the URL with your backend API endpoint
      const backendURL = "http://localhost:3000/addTag";

      // Replace this with the data you want to send to the backend
      const dataToSend = {
        id: "bbbbb",
        name: "sugar",
        isTrue: "true",
      };

      axios
        .post(backendURL, dataToSend)
        .then((response) => {
          console.log("Backend response:", response.data);
        })
        .catch((error) => {
          console.error("Error sending data to the backend:", error);
        });
    },

    add() {
      var item: ListItem = {
        id: this.id,
        name: this.name,
        isTrue: this.isTrue,
      };
      this.list.push(item);
      this.id = "";
      this.name = "";
      this.isTrue = true;
    },

    del(id: string) {
      var index = this.list.findIndex((item) => {
        if (item.id == id) {
          return true;
        }
      });
      this.list.splice(index, 1);
    },
  },
  search(keywords: string) {
    return this.list.filter((item) => {
      if (item.name.includes(keywords)) {
        return item;
      }
    });
  },
};
</script>
