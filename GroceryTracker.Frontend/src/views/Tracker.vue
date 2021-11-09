<template>
   <div class="home">
      <input id="central-search" type="text" v-model="inputText" />
      <div>
         <ul>
            <li v-for="(item, index) in recent" :key="index">
               {{ item.articleName }}
            </li>
         </ul>
         <ul>
            <li v-for="(item, index) in frequent" :key="index">
               {{ item.articleName }}
            </li>
         </ul>
      </div>
   </div>
</template>

<script lang="ts">
import { Options, Vue, Watch } from "vue-property-decorator";

@Options({
   components: {},
})
export default class Tracker extends Vue {
   private inputText = "";
   private recent: { timestamp: Date; articleName: string; brandName: string; details: string }[] = [];
   private frequent: { purchaseCount: number; articleName: string; brandName: string; details: string }[] = [];

   public async created() {
      let recentResponse = await fetch("https://localhost:5001/purchase/recent?marketId=9");
      let data = await recentResponse.json();
      this.recent = data.map((x: any) => {
         return {
            timestamp: new Date(x.timestamp),
            articleName: x.articleName,
            brandName: x.brandName,
            details: x.details,
         };
      });

      let frequentResponse = await fetch("https://localhost:5001/purchase/frequent?marketId=9");
      let frequentData = await frequentResponse.json();
      this.frequent = frequentData.map((x: any) => {
         return {
            purchaseCount: x.purchaseCount,
            articleName: x.articleName,
            brandName: x.brandName,
            details: x.details,
         };
      });
   }

   @Watch("inputText")
   async onInput(value: string) {
      let response = await fetch("https://localhost:5001/purchase/frequent?marketId=9");
      let data = await response.json();

      console.log(data);
   }
}
</script>

<style scoped>
#central-search {
   width: 50%;
   height: 100%;
   border: none;
   border-radius: 5px;
   padding: 10px;
   font-size: 1.5em;
   font-family: "Roboto", sans-serif;
   font-weight: bold;
   text-align: center;
   color: #333;
   background-color: white;
   box-shadow: 0px 0px 50px #000;
}
</style>
