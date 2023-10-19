<template>
  <div class="app">
    <div class = "panel panel-primary">
      <div class = "panel-heading">
        <h3 class = "panel-title">Tag Manager</h3>
      </div>
      <div class = "panel-body form-inline">
        <label>
          ID:
          <input type = "text" class = "form-control" v-model="id">
        </label>
        <label>
          Name:
          <input type = "text" class = "form-control" v-model="name">
        </label>
        <input type = "button" value = "add" class = "btn btn-primary" @click="add()">
      </div>
    </div>
    <table class = "table table-bordered table honver table-striped">
      <thead>
        <th>Id</th>
        <th>Name</th>
        <th>Time</th>
        <th>Operation</th>
      </thead>
      <tbody>
        <tr v-for="item in list" :key="item.id">
          <td>{{ item.id }}</td>
          <td>{{ item.name }}</td>
          <td>{{ item.ctime }}</td>
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
</template>

<script lang="ts">
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
export default {
  name: '#app',
  props:{
    msg:String
  },
  data(){
    return{
      id: '',
      name: '',
      keywords: '',
      list:[
        {id: 1, name: 'paper',ctime: new Date()},
        {id: 2, name: 'bottle',ctime: new Date()},
        {id: 3, name: 'mouse',ctime: new Date()},
        {id: 4, name: 'pen',ctime: new Date()},
        {id: 5, name: 'computer',ctime: new Date()},
      ]
    }
  },
  method:{
    add(){
      var item = {id:this.id,name:this.name,ctime: new Date()}
      this.list.push(item)
      this.id = ''
      this.name = ''
    },

    del(id){
      var index = this.list.findIndex(item =>{
        if(item.id == id){
          return true;
        }
      })
      this.list.splice(index, 1)
    }
  },
  search(keywords){
    return this.list.filter(item =>{
      if(item.name.includes(keywords)){
        return item
      }
    })
  }

}

</script>