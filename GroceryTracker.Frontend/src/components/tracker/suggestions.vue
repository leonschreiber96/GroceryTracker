<template>
   <div class="wrapper">
      <h3>{{ title }}</h3>
      <ul class="suggestions-list">
         <li v-for="(item, index) in suggestions" :key="'suggestion' + index">
            <article-suggestion
               :articleId="item.articleId"
               :articleName="item.articleName"
               :brandName="item.brandName"
               :details="item.details"
               :isSelected="item.selected"
               @selected="select"
            />
         </li>
      </ul>
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue, Watch } from "vue-property-decorator"
import Suggestion from "../../model/suggestion"

import ArticleSuggestion from "./articleSuggestion.vue"

@Options({
   components: { ArticleSuggestion },
   name: "suggestions",
})
export default class Suggestions extends Vue {
   @Prop({ default: [] })
   suggestions!: Suggestion[]

   @Prop({ default: "" })
   title!: string

   public select(suggestion?: Suggestion) {
      if (suggestion) {
         this.$emit("selected", suggestion)
      }
   }
}
</script>

<style scoped>
.wrapper {
   flex: 1;
}

.suggestions-list {
   margin: 10px 30px;

   list-style: none;
   padding-inline: 0;
}
</style>
