<template>
  <div class="hello">
    <div class = "panel panel-primary">
      <div class = "panel-heading">
        <h3 class = "panel-title">Add tag</h3>
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
        <input type = "button" value = "add" class = "btn btn-primary">
        <label>
          Keywords:
          <input type = "text" class = "form-control" v-model="keywords">
        </label>
      </div>
    </div>
    <table class = "table table-bordered table honver table-striped">
      <thead>
        <th>Id</th>
        <th>Name</th>
        <th>Time</th>
        <th>Operate</th>
      </thead>
    </table>
  </div> 
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  name: 'HelloWorld',
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
      ]
    }
  },
  method:{
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

})

</script>

<style scoped>
@import  "bootstrap/dist/css/bootstrap.min.css"

</style>