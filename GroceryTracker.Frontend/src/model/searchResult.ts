import { SearchResultDto } from "@/dtos/searchResultsDto";

export default interface SearchResult extends SearchResultDto {
   selected: boolean;
}
