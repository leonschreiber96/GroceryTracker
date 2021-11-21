<template>
   <div id="tracker">
      <div id="tracker-main">
         <div id="central-search-container">
            <input id="central-search" type="text" autofocus ref="search" v-model="searchText" @keyup="search" />
         </div>
         <keep-alive>
            <suggestions-container id="suggestions-container" @LeaveTop="focusSearch()" :openForSelection="true" v-if="searchText == ''" />
         </keep-alive>
         <keep-alive>
            <search-result-container v-if="searchText != ''" :results="searchResults.results" :search="searchResults.search" />
         </keep-alive>
      </div>
      <div id="tracker-side">Warenkorb</div>
   </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-property-decorator";
import SuggestionsContainer from "../components/tracker/suggestionsContainer.vue";
import SearchResultContainer from "../components/tracker/searchResultContainer.vue";

import { post } from "../helpers";
import { SearchResultsDto } from "@/dtos/searchResultsDto";

@Options({
   components: { SuggestionsContainer, SearchResultContainer },
   name: "tracker",
})
export default class Tracker extends Vue {
   private searchText = "";
   private searchResults: SearchResultsDto = {
      search: { articleName: "", originalSearchString: "", details: "", brandName: "", primaryCategory: "", dynamicSearchString: "", resultLimit: 10 },
      results: [],
   };

   private async search() {
      let results = await post<string, SearchResultsDto>("https://localhost:5001/search", this.searchText);
      this.searchResults = results;
   }

   private focusSearch() {
      (this.$refs.search as HTMLInputElement).focus();
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
