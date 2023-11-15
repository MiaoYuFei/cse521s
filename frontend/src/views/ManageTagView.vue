<template>
  <div class="app">
    <div class="panel panel-primary">
      <div class="panel-heading">
        <h3 class="panel-title">Tag Manager</h3>
      </div>
      <div class="panel-body form-inline">
        <label>
          ID:
          <input type="text" class="form-control" v-model="id" />
        </label>
        <label>
          Name:
          <input type="text" class="form-control" v-model="name" />
        </label>
        <input
          type="button"
          value="add"
          class="btn btn-primary"
          @click="add()"
        />
      </div>
    </div>
    <table class="table table-bordered table honver table-striped">
      <thead>
        <th>Id</th>
        <th>Name</th>
        <th>IsDistractor</th>
        <th>Operation</th>
      </thead>
      <tbody>
        <tr v-for="item in list" :key="item.id">
          <td>{{ item.id }}</td>
          <td>{{ item.name }}</td>
          <td>{{ item.isTrue}}</td>
          <td>
            <a href="#" @click.prevent="del(item.id)">Delete</a>
          </td>
          <td>
            <a href="#">Edit</a>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h1>Tag IDs</h1>
    <ul>
      <li v-for="tagId in tagIds" :key="tagId">{{ tagId }}</li>
    </ul>
  </div>
</template>

import axios from 'axios';

<script lang="ts">
interface ListItem {
  id: string;
  name: string;
  isTrue: boolean;
}
export default {
  name: "ManageTagView",
  props: {
    msg: String,
  },
  data() {
    return {
      tagIds: [],
      id: "",
      name: "",
      keywords: "",
      list: [
        { id: "1", name: "paper", isTrue: true },
        { id: "2", name: "bottle", isTrue: false },
        { id: "3", name: "mouse",  isTrue: false },
        { id: "4", name: "pen", isTrue: true },
        { id: "5", name: "computer", isTrue: true },
      ] as ListItem[],
    };
  },

  mounted() {
    // Fetch tag IDs every second
    this.fetchTagIds();
    setInterval(this.fetchTagIds, 1000);
  },

  methods: {
    async fetchTagIds() {
      try {
        // actual backend API endpoint
        const response = await axios.get('http://localhost:3000/getAllTags');
        this.tagIds = response.data; //  an array of tag IDs
      } catch (error) {
        console.error('Error fetching tag IDs:', error);
      }
    },

    sendData() {
      // Replace the URL with your backend API endpoint
      const backendURL = 'http://localhost:3000/addTag';

      // Replace this with the data you want to send to the backend
      const dataToSend = {
        id: 'bbbbb',
        name: 'sugar',
        isTrue: 'true',
      };

      axios.post(backendURL, dataToSend)
        .then(response => {
          console.log('Backend response:', response.data);
        })
        .catch(error => {
          console.error('Error sending data to the backend:', error);
        });
    },

    add() {
      var item: ListItem = { id: this.id, name: this.name, isTrue: this.isTrue};
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
