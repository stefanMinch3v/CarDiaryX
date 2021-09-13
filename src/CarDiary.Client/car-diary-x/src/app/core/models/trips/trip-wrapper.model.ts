import { TripOutputModel } from './trip-output.model';

export interface TripWrapperModel {
  trips: Array<TripOutputModel>;
  totalCount: number;
}