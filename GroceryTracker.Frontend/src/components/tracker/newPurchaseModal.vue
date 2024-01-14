<template>
   <div class="modal">
      <div class="card">
         <form>
            <div class="form-group col">
               <label for="articleName">Article Name</label>
               <input
                  type="text"
                  id="articleName"
                  placeholder="Enter article name"
                  v-model="selectedPurchase.articleName"
               />
            </div>
            <div class="form-group col">
               <label for="details">Details</label>
               <input type="text" id="details" placeholder="Enter details" v-model="selectedPurchase.details" />
            </div>
            <div class="form-group col">
               <label for="brandName">Brand Name</label>
               <input type="text" id="brandName" placeholder="Enter brand name" v-model="selectedPurchase.brandName" />
            </div>
            <div class="separator"></div>
            <div class="form-group row">
               <label for="unit-price">Price per unit</label>
               <div class="flex">
                  <input type="number" id="unit-price" placeholder="--" v-model="unitPrice" />
                  <span class="material-symbols-outlined unit-symbol"> euro_symbol </span>
               </div>
            </div>
            <div class="form-group row">
               <label for="pfand">Pfand</label>
               <input type="text" id="pfand" placeholder="--" v-model="pfand" />
            </div>
            <div class="form-group row">
               <label for="amount-kg">Amount / Kg</label>
               <input type="text" id="amount-kg" placeholder="0.00" v-model="amount" />
            </div>
         </form>
         <div class="flex">
            <span class="material-icons"> price_check </span>
            <span>{{ totalPrice ?? "--" }}</span>
         </div>
         <div class="btn-container flex">
            <button class="btn btn-secondary" @click="cancel">Cancel</button>
            <button class="btn btn-primary" @click="save">Save</button>
         </div>
      </div>
   </div>
</template>

<script lang="ts">
// import PurchaseOverviewDto from "@/dtos/purchaseOverviewDto"
import PurchaseOverviewDto from "@/dtos/purchaseOverviewDto"
import NewPurchaseDto from "@/dtos/newPurchaseDto"
import { Options, Prop, Vue } from "vue-property-decorator"
import { SearchResultDto } from "@/dtos/searchResultsDto"

@Options({
   components: {},
   name: "newPurchaseModal",
})
export default class NewPurchaseModal extends Vue {
   public articleName?: string
   public details?: string
   public brand?: string
   public unitPrice?: number
   public pfand?: number
   public amount?: number

   @Prop({
      default: {
         articleName: "",
         details: "",
         brandName: "",
         articleId: 0,
      },
   })
   selectedPurchase!: PurchaseOverviewDto | SearchResultDto

   public get totalPrice() {
      if (this.unitPrice == undefined || this.amount == undefined) return undefined

      return this.unitPrice * this.amount + (this.pfand ?? 0)
   }

   public cancel() {
      this.$emit("cancel")
   }

   public save() {
      const purchase = undefined

      this.$emit("save", purchase)
   }
}
</script>

<style lang="scss" scoped>
.card {
   position: relative;
   display: flex;
   flex-direction: column;
   justify-content: center;
   align-items: center;
}

.separator {
   height: 1px;
   width: 100%;
   background-color: #959191;
}

.form-group {
   margin: 1em 0;
   &.col {
      display: flex;
      flex-direction: column;

      label {
         text-align: left;
      }
   }

   &.row {
      display: flex;
      flex-direction: row;
      justify-content: space-between;

      label {
         margin-right: 2em;
      }
   }

   label {
      font-weight: bold;
   }

   input {
      border-radius: 5px;
      border: none;
      padding: 5px;
      background-color: #e9e9e9;

      &[type="number"] {
         width: 50px;
         padding-right: 2em;

         text-align: right;

         // Remove arrows
         -moz-appearance: textfield;
         &::-webkit-outer-spin-button,
         &::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
         }
      }
   }
}

.unit-symbol {
   font-size: 1.2em;
   margin-left: -1.2em;
   opacity: 0.5;
}

.btn-container {
   justify-content: space-between;
   align-self: flex-end;
   justify-self: end;
}
</style>
