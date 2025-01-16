export class Pagination {
  currentPage = {} as number;
  itemsPerPage = {} as number;
  totalItems = {} as number;
  totalPages = {} as number;


}

export class PaginatedResult<T> {
  result = {} as T;
  pagination = {} as Pagination ;

}
