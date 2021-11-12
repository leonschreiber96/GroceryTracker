export interface SearchResultsDto {
   search: Search;
   results: SearchResultDto[];
}

export interface Search {
   originalSearchString: string;
   articleName: string;
   brandName: string;
   primaryCategory: string;
   details: string;
   dynamicSearchString: string;
   resultLimit: 10;
}

export interface SearchResultDto {
   articleName: string;
   brandName: string;
   details: string;
   categoryName: string;
   lastPrice: number;
   averagePrice: number;
}
