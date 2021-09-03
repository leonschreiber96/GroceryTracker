<template>
   <div>Was geht ab</div>
   <div>Ich bin Leon und sage: "{{ msg }}"</div>
   <input type="text" v-model="inputText" />
   <div>{{ greeting }}</div>
</template>

<script lang="ts">
import { Options, Vue, Prop } from "vue-property-decorator";

@Options({
   name: "hello-world",
})
export default class HelloWorld extends Vue {
   @Prop({ default: "lol", type: String }) readonly msg!: string;
   private inputText = "";

   get greeting() {
      return `Hello ${this.inputText}`;
   }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
   margin: 40px 0 0;
}
ul {
   list-style-type: none;
   padding: 0;
}
li {
   display: inline-block;
   margin: 0 10px;
}
a {
   color: #42b983;
}
</style>
