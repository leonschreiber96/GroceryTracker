<template>
   <!-- <div id="nav">
      <router-link to="/">Home</router-link>
      <router-link to="/about">About</router-link>
   </div> -->
   <router-view />
</template>

<style lang="scss">
body {
   background: #e5e5e5;
   margin: 0;

   overflow: hidden;
}

#app {
   height: 100vh;
   max-height: 100%;

   font-family: Roboto, Avenir, Helvetica, Arial, sans-serif;
   -webkit-font-smoothing: antialiased;
   -moz-osx-font-smoothing: grayscale;
   text-align: center;

   background-color: inherit;
   color: #2c3e50;
}

#nav {
   padding: 30px;

   a {
      font-weight: bold;
      color: #2c3e50;

      &.router-link-exact-active {
         color: #42b983;
      }
   }
}
</style>
