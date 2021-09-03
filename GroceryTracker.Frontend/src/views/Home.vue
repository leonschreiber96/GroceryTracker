<template>
   <div class="home">
      <img alt="Vue logo" src="../assets/logo.png" />
      <hello-world />
   </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-property-decorator";
import HelloWorld from "@/components/HelloWorld.vue";

@Options({
   components: {
      HelloWorld,
   },
})
export default class Home extends Vue {}
</script>
