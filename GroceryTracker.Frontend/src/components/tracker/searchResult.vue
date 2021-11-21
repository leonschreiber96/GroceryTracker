<template>
   <tr :class="'search-result ' + (isSelected ? 'selected' : '')">
      <td class="article-name">{{ result.articleName }}</td>
      <td>{{ result.details }}</td>
      <td>{{ result.brandName }}</td>
      <td>{{ result.categoryName }}</td>
      <td>{{ result.lastPrice }}€ / {{ result.averagePrice }}€</td>
   </tr>
</template>

<script lang="ts">
import { Search, SearchResultDto } from "@/dtos/searchResultsDto";
import { Options, Prop, Vue } from "vue-property-decorator";

@Options({
   components: {},
   name: "SearchResult",
})
export default class SearchResult extends Vue {
   @Prop()
   result!: SearchResultDto;

   @Prop({ default: "" })
   search!: Search;

   @Prop({ default: false })
   isSelected!: boolean;

   get labels(): { article: string; brand: string; details: string; category: string } {
      return {
         article: this.result.articleName.replace(`(${this.search.articleName})`, this.search.articleName.bold()),
         brand: this.result.brandName.replace(`(${this.search.brandName})`, this.search.brandName.bold()),
         details: this.result.details.replace(`(${this.search.details})`, this.search.details.bold()),
         category: this.result.categoryName.replace(`(${this.search.primaryCategory})`, this.search.primaryCategory.bold()),
      };
   }
}
</script>

<style lang="scss" scoped>
$search-result-border-radius: 6px;

.search-result {
   border-radius: $search-result-border-radius;
   padding: 0.2em 1em;

   box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);

   cursor: pointer;
   background: #ffffff;

   &:hover {
      background-color: #d2d2d2;
   }

   td {
      padding: 0.6em 0.5em;
   }

   td:first-child {
      border-top-left-radius: $search-result-border-radius;
      border-bottom-left-radius: $search-result-border-radius;
   }

   td:last-child {
      border-top-right-radius: $search-result-border-radius;
      border-bottom-right-radius: $search-result-border-radius;
   }
}

.article-name {
   align-self: flex-start;
   flex: 2;

   overflow-x: hidden;
   text-overflow: ellipsis;
   white-space: nowrap;

   line-height: 24px;
   font-size: 16px;
   color: #111;
   text-align: left;
}

.brand-name {
   /* margin-left: 1em; */

   align-self: flex-end;
   flex: 1;

   overflow-x: hidden;
   text-overflow: ellipsis;
   white-space: nowrap;

   line-height: 24px;
   font-size: 17px;
   font-weight: bold;
   text-align: right;
   color: #808080;
}

.selected {
   background-color: #d2d2d2;
}
</style>
