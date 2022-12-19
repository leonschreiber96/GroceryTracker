<template>
   <div id="purchase-list">
      <p v-for="purchase in purchases" :key="purchase">{{ purchase }}</p>
   </div>
</template>

<script lang="ts">
import { Options, Prop, Vue } from "vue-property-decorator"

@Options({
   components: {},
   name: "PurchaseList",
})
export default class PurchaseList extends Vue {
   @Prop({ default: [] })
   purchases!: any[]
}
</script>

<style lang="scss" scoped></style>
