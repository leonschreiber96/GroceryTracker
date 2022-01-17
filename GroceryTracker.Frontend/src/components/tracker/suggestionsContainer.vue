<template>
   <div class="suggestions-container">
      <suggestions :suggestions="recent" title="Zuletzt gekauft" />
      <suggestions :suggestions="frequent" title="Häufig gekauft" />
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue, Watch } from "vue-property-decorator";
import Suggestion from "@/model/suggestion";
import { get } from "@/helpers";

import Suggestions from "./suggestions.vue";

@Options({
   components: { Suggestions },
   name: "SuggestionsContainer",
})
export default class SuggestionsContainer extends Vue {
   private recent: Suggestion[] = [];
   private frequent: Suggestion[] = [];
   private selected?: Suggestion = undefined;

   private selectionIndex = -1;

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

      if (this.selectionIndex >= 0) [...this.recent, ...this.frequent][this.selectionIndex].selected = false;
      this.selectionIndex = -1;
   }

   public deactivated() {
      document.removeEventListener("keydown", this.onKeyDown);
   }

   private onKeyDown(event: KeyboardEvent) {
      event.stopPropagation();

      if (!["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Enter"].includes(event.key)) {
         return;
      }

      if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(event.key)) {
         if (this.selectionIndex >= 0) [...this.recent, ...this.frequent][this.selectionIndex].selected = false;

         if (event.key === "ArrowUp") {
            this.selectionIndex--;
         } else if (event.key === "ArrowDown") {
            this.selectionIndex++;
         } else if (event.key === "ArrowLeft") {
            this.selectionIndex += this.selectionIndex < this.recent.length ? this.recent.length : -this.frequent.length;
         } else if (event.key === "ArrowRight") {
            this.selectionIndex += this.selectionIndex >= this.recent.length ? -this.recent.length : this.frequent.length;
         }

         if (this.selectionIndex >= 0) [...this.recent, ...this.frequent][this.selectionIndex].selected = true;
         else this.$emit("leavetop");
      } else if (event.key === "Enter") {
         if (this.selectionIndex >= 0) {
            this.$emit("select", [...this.recent, ...this.frequent][this.selectionIndex]);
         }
      }
   }

   private emitSelection() {
      if (this.selected) {
         this.$emit("select", this.selected);
      }
   }

   private async fetchSuggestions() {
      let recent = get<Suggestion[]>("https://localhost:5001/purchase/recent?marketId=9");
      let frequent = get<Suggestion[]>("https://localhost:5001/purchase/frequent?marketId=9");

      [this.frequent, this.recent] = await Promise.all([frequent, recent]);
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
