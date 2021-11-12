<template>
   <div id="search-result-container" style="margin-top: 150px">
      <ul>
         <li v-for="(result, index) in results" :key="index">{{ result.articleName }} - {{ result.details }} ({{ result.brandName }})</li>
      </ul>
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue, Watch } from "vue-property-decorator";
import { SearchResultDto } from "../../dtos/searchResultsDto";

@Options({
   components: {},
   name: "SearchResultContainer",
})
export default class SearchResultContainer extends Vue {
   @Prop({ default: [] })
   results!: SearchResultDto[];
}
</script>

<style scoped>
.suggestions-container {
   display: flex;
   flex-direction: row;
   justify-content: center;
   align-items: center;
}
</style>
