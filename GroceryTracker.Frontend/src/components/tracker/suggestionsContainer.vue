<template>
   <div class="suggestions-container">
      <suggestions :suggestions="recent" title="Zuletzt gekauft" />
      <suggestions :suggestions="frequent" title="Häufig gekauft" />
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue, Watch } from "vue-property-decorator";
import Suggestion from "../../model/suggestion";
import { get } from "../../helpers";

import Suggestions from "./suggestions.vue";

@Options({
   components: { Suggestions },
   name: "SuggestionsContainer",
})
export default class SuggestionsContainer extends Vue {
   private recent: Suggestion[] = [];
   private frequent: Suggestion[] = [];

   @Prop({ default: false })
   openForSelection!: boolean;

   mounted() {
      this.onOpenForSelectionChanged(this.openForSelection);
   }

   @Watch("openForSelection")
   onOpenForSelectionChanged(newValue: boolean) {
      if (newValue) {
         document.addEventListener("keydown", this.onKeyDown);
      } else {
         document.removeEventListener("keydown", this.onKeyDown);
      }
   }

   async created() {
      await this.fetchSuggestions();
   }

   activated() {
      if (this.openForSelection) {
         document.addEventListener("keydown", this.onKeyDown);
      }
   }

   deactivated() {
      document.removeEventListener("keydown", this.onKeyDown);
   }

   private onKeyDown(event: KeyboardEvent) {
      event.stopPropagation();
      if ([...this.frequent, ...this.recent].some((s) => s.selected)) {
         let recentIndex = this.recent.findIndex((s) => s.selected);
         let frequentIndex = this.frequent.findIndex((s) => s.selected);

         let selectingList = recentIndex >= 0 ? this.recent : this.frequent;
         let otherList = recentIndex >= 0 ? this.frequent : this.recent;
         let index = recentIndex >= 0 ? recentIndex : frequentIndex;

         if (event.key === "ArrowDown") {
            selectingList[index].selected = false;
            index = (index + 1) % selectingList.length;
            selectingList[index].selected = true;
         } else if (event.key === "ArrowUp") {
            selectingList[index].selected = false;
            index = index - 1;
            if (index >= 0) {
               selectingList[index].selected = true;
            } else {
               this.$emit("LeaveTop");
            }
         } else if (event.key === "ArrowLeft" || event.key === "ArrowRight") {
            selectingList[index].selected = false;
            otherList[index].selected = true;
         } else if (event.key === "Escape") {
            selectingList[index].selected = false;
            this.$emit("LeaveTop");
         }
      } else if (event.key === "ArrowDown") {
         this.recent[0].selected = true;
      }
   }

   private async fetchSuggestions() {
      this.recent = await get<Suggestion[]>("https://localhost:5001/purchase/recent?marketId=9");
      this.frequent = await get<Suggestion[]>("https://localhost:5001/purchase/frequent?marketId=9");
   }
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
