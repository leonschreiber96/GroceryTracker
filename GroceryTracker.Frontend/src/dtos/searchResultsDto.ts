export interface SearchResultsDto {
   search: Search
   results: SearchResultDto[]
}

export interface Search {
   originalSearchString: string
   articleName: string
   brandName: string
   primaryCategory: string
   details: string
   dynamicSearchString: string
   resultLimit: 10
}

export interface SearchResultDto {
   articleName: string
   brandName: string
   details: string
   categoryName: string
   articleMatch: SearchMatchType
   detailsMatch: SearchMatchType
   brandMatch: SearchMatchType
   categoryMatch: SearchMatchType
   lastPrice: number
   averagePrice: number
}

export enum SearchMatchType {
   Query,
   Dynamic,
   NoMatch,
}
