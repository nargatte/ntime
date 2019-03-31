interface IDtoBase<T> {
  Id: number;
  copyDataFromDto(entity: T): void;
}
