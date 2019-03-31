interface IDtoBase<T> {
  Id: number;
  CopyDataFromDto(entity: T): void;
}
