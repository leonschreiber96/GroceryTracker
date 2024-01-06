<template>
   <div id="purchase-list">
      <div v-for="purchase in purchases" :key="purchase.articleId" class="purchase">
         <div class="top-bar">
            <span> {{ purchase.articleName }} {{ purchase.details }} </span>
            <span class="material-icons" @click="() => deletePurchase(purchase)"> delete_outline </span>
         </div>
         <span class="brand">
            {{ purchase.brandName }}
         </span>
         <!-- <div class="tags">
            <div v-for="purchaseDetail in purchase.tags" :key="purchaseDetail">
               {{ purchaseDetail }}
            </div>
         </div> -->
      </div>
   </div>
</template>

<script lang="ts">
import PurchaseOverviewDto from "@/dtos/purchaseOverviewDto"
import { Options, Prop, Vue } from "vue-property-decorator"

@Options({
   components: {},
   name: "PurchaseList",
})
export default class PurchaseList extends Vue {
   @Prop({ default: [] })
   purchases!: PurchaseOverviewDto[]

   public deletePurchase(purchase: PurchaseOverviewDto) {
      console.log("delete purchase", purchase)
      this.$emit("deletePurchase", purchase)
   }
}
</script>

<style lang="scss" scoped>
.purchase {
   position: relative;

   border-radius: 5px;
   box-shadow: 0 0 3px 0 rgba(0, 0, 0, 0.5);
   padding: 5px;
   margin: 5px;

   .top-bar {
      display: block;
      display: flex;

      span:first-child {
         flex-grow: 1;
         text-align: left;
         font-weight: bold;
      }
      span:nth-child(2) {
         cursor: pointer;
         &:hover {
            color: rgb(141, 63, 63);
         }
         transition: color 0.2s ease-in-out;
      }
   }

   .brand {
      display: block;
      text-align: left;
      font-size: 0.8em;
   }

   .tags {
      display: flex;
      flex-wrap: wrap;
      justify-content: flex-start;

      div {
         display: inline-block;

         background-color: #c7e0c0;
         border-radius: 10px;
         padding: 2px 5px;
         margin: 2px;
         font-size: 0.8em;
      }
   }
}
</style>
