<template>
   <div class="search-result-container" style="margin-top: 150px">
      <!-- <div style="background: red; height: 200px; position: absolute"></div> -->
      <table>
         <thead>
            <tr>
               <td>Artikel</td>
               <td>Details</td>
               <td>Marke</td>
               <td>Kategorie</td>
               <td>Preis Zuletzt/Durchschnitt</td>
            </tr>
         </thead>
         <tbody>
            <tr>
               <td :class="'btn-new-article' + (selectionIndex == -1 ? ' selected' : '')" colspan="5">
                  <div>Neuen Artikel hinzufügen</div>
               </td>
            </tr>
            <article-search-result
               :result="result"
               :search="search"
               v-for="(result, index) in results"
               :key="'search' + index"
            />
         </tbody>
      </table>
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue } from "vue-property-decorator"
import { Search } from "@/dtos/searchResultsDto"
import SearchResult from "@/model/searchResult"
import ArticleSearchResult from "./articleSearchResult.vue"

@Options({
   components: { ArticleSearchResult },
   name: "SearchResultContainer",
})
export default class SearchResultContainer extends Vue {
   @Prop({ default: [] })
   results!: SearchResult[]

   @Prop()
   search!: Search

   public selectionIndex = -2

   public activated() {
      document.addEventListener("keydown", this.onKeyDown)
      if (this.selectionIndex >= 0) this.results[this.selectionIndex].selected = false
      this.selectionIndex = -2
   }

   public deactivated() {
      document.removeEventListener("keydown", this.onKeyDown)
   }

   private onKeyDown(event: KeyboardEvent) {
      event.stopPropagation()

      if (!["ArrowUp", "ArrowDown", "Enter"].includes(event.key)) {
         return
      }

      if (this.results.length === 0) {
         return
      }

      if (event.key === "ArrowDown") {
         if (this.selectionIndex >= 0) this.results[this.selectionIndex].selected = false
         this.selectionIndex++
         if (this.selectionIndex >= 0) this.results[this.selectionIndex].selected = true
      } else if (event.key === "ArrowUp") {
         if (this.selectionIndex >= 0) this.results[this.selectionIndex].selected = false
         this.selectionIndex--
         if (this.selectionIndex >= 0) this.results[this.selectionIndex].selected = true
      } else if (event.key === "Enter") {
         if (this.selectionIndex >= 0) {
            this.$emit("select", this.results[this.selectionIndex])
         }
      }
   }
}
</script>

<style lang="scss" scoped>
.search-result-container {
   margin: 0 3em;

   display: flex;
   flex-direction: row;
   justify-content: center;
   align-items: center;

   background-color: inherit;
}

.btn-new-article {
   border-radius: 5px;
   border-radius: 6px;

   background-image: url("data:image/svg+xml,%3csvg width='100%25' height='100%25' xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' rx='6' ry='6' stroke='gray' stroke-width='4' stroke-dasharray='10%2c10' stroke-dashoffset='16' stroke-linecap='round'/%3e%3c/svg%3e");

   line-height: 2.5em;

   cursor: pointer;
   text-align: center;

   div {
      padding: 1em 0;
      line-height: 100%;
   }

   &.selected {
      background-color: #929292;
   }

   &:hover {
      background-color: #d2d2d2;
   }
}

table {
   border-spacing: 0 5px;

   background-color: inherit;
   text-align: left;
}

thead {
   background-color: inherit;
   font-size: 1.3em;
   font-weight: bold;

   td {
      padding: 0.5em 0;
   }
}

tbody:before {
   line-height: 1em;
   content: "\200C";
   display: block;
}
</style>
