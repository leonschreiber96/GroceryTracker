<template>
   <div :class="'article-suggestion ' + (isSelected ? 'selected' : '')">
      <span class="article-name"
         >{{ articleName }} <span v-if="details"> - {{ details }}</span></span
      >
      <span class="brand-name">{{ brandName }}</span>
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue } from "vue-property-decorator";

@Options({
   components: {},
   name: "ArticleSuggestion",
})
export default class ArticleSuggestion extends Vue {
   @Prop({ default: "" })
   articleName!: string;

   @Prop({ default: "" })
   brandName!: string;

   @Prop({ default: "" })
   details!: string;

   @Prop({ default: false })
   isSelected!: boolean;
}
</script>

<style lang="scss" scoped>
.article-suggestion {
   margin: 0.38em 0;
   padding: 0.2em 1em;
   max-width: 30em;

   display: flex;

   background: #ffffff;
   box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.25);
   border-radius: 4px;

   cursor: pointer;

   &:hover {
      background-color: #d2d2d2;
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
