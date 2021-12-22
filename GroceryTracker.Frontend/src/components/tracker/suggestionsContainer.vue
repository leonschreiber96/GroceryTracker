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
   private selected?: Suggestion = undefined;

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
         this.selected = undefined;
         this.recent.forEach((x) => (x.selected = false));
         this.frequent.forEach((x) => (x.selected = false));
      }
   }

   public async created() {
      await this.fetchSuggestions();
   }

   public activated() {
      if (this.openForSelection) {
         document.addEventListener("keydown", this.onKeyDown);
      }
   }

   public deactivated() {
      document.removeEventListener("keydown", this.onKeyDown);
   }

   private onKeyDown(event: KeyboardEvent) {
      console.log("lel");
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
            this.selected = selectingList[index];
         } else if (event.key === "ArrowUp") {
            selectingList[index].selected = false;
            index = index - 1;
            if (index >= 0) {
               selectingList[index].selected = true;
               this.selected = selectingList[index];
            } else {
               this.selected = undefined;
               this.$emit("LeaveTop");
            }
         } else if (event.key === "ArrowLeft" || event.key === "ArrowRight") {
            selectingList[index].selected = false;
            otherList[index].selected = true;
            this.selected = otherList[index];
         } else if (event.key === "Escape") {
            selectingList[index].selected = false;
            this.selected = undefined;
            this.$emit("LeaveTop");
         }
      } else if (event.key === "ArrowDown") {
         this.recent[0].selected = true;
         this.selected = this.recent[0];
      } else if (event.key === "Enter") {
         this.emitSelection();
      }
   }

   private emitSelection() {
      console.log("Emit Selection");
      if (this.selected) {
         this.$emit("Select", this.selected);
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
