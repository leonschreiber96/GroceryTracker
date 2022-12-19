<template>
   <div id="tracker">
      <div id="tracker-main">
         <div id="central-search-container">
            <input id="central-search" type="text" autofocus ref="search" v-model="searchText" />
         </div>
         <keep-alive>
            <suggestions-container
               key="suggestions"
               id="suggestions-container"
               @leavetop="focusSearch"
               @select="selectSuggestion"
               :openForSelection="typingSearch"
               v-if="searchText === ''"
            />
         </keep-alive>
         <keep-alive>
            <search-result-container
               key="searchresults"
               v-if="searchText != ''"
               :results="searchResults.results"
               :search="searchResults.search"
               @select="selectSearchResult"
            />
         </keep-alive>
      </div>
      <div id="tracker-side">
         <purchase-list :purchases="purchases"> </purchase-list>
      </div>
   </div>
</template>

<script lang="ts">
import { Options, Vue, Watch } from "vue-property-decorator"
import SuggestionsContainer from "../components/tracker/suggestionsContainer.vue"
import SearchResultContainer from "../components/tracker/searchResultContainer.vue"
import PurchaseList from "../components/tracker/purchaseList.vue"

import { post } from "../helpers"
import { SearchResultsDto } from "@/dtos/searchResultsDto"
import Suggestion from "@/model/suggestion"
import SearchResult from "@/model/searchResult"

@Options({
   components: { SuggestionsContainer, SearchResultContainer, PurchaseList },
   name: "tracker",
})
export default class Tracker extends Vue {
   public searchText = ""
   public searchResults: SearchResultsDto = {
      search: {
         articleName: "",
         originalSearchString: "",
         details: "",
         brandName: "",
         primaryCategory: "",
         dynamicSearchString: "",
         resultLimit: 10,
      },
      results: [],
   }
   public typingSearch = false
   public purchases: string[] = []

   public async created() {
      document.addEventListener("keydown", (args) => {
         if (!["ArrowDown", "ArrowUp", "ArrowLeft", "ArrowRight", "Enter"].includes(args.key)) {
            this.focusSearch()
         } else {
            this.typingSearch = true
         }
      })
   }

   public selectSuggestion(suggestion: Suggestion): void {
      console.log("Select Suggestion")
      console.log(suggestion)
      this.purchases.push(suggestion.articleName)
   }

   public selectSearchResult(searchResult: SearchResult): void {
      console.log("Select Search result")
      console.log(searchResult)
      this.purchases.push(searchResult.articleName)
   }

   @Watch("searchText")
   private async search() {
      let results = await post<string, SearchResultsDto>("http://localhost:3000/search", this.searchText)
      this.searchResults = results
   }

   public focusSearch(): void {
      ;(this.$refs.search as HTMLInputElement)?.focus()
      this.typingSearch = true
   }
}
</script>

<style lang="scss" scoped>
$search-bar-padding: 40px;
$search-bar-height: 50px;

$main-screen-ratio: 3;
$side-screen-ratio: 1;

#central-search-container {
   padding: $search-bar-padding 0;
   width: 100% / ($main-screen-ratio + $side-screen-ratio) * $main-screen-ratio;
   position: fixed;
   background-color: inherit;
}

#central-search {
   position: relative;
   width: 50%;
   height: $search-bar-height;

   border: none;
   border-radius: 20px;
   background-color: white;
   filter: drop-shadow(0px 4px 4px rgba(0, 0, 0, 0.25));

   font-size: 1.5em;
   text-align: center;
   color: #333;
}

#suggestions-container {
   margin: ($search-bar-height + $search-bar-padding * 2) 50px 50px 50px;
}

#tracker {
   display: flex;
   height: 100%;
   max-height: 100vh;

   background-color: inherit;
}

#tracker-main {
   flex: $main-screen-ratio;
   overflow-y: scroll;

   background-color: inherit;

   &::-webkit-scrollbar {
      display: none;
   }
}

#tracker-side {
   flex: $side-screen-ratio;
   background-color: white;
}
</style>
