import { AddressModel } from './address.model';
import { TripInputModel } from './trip-input.model';

export interface TripOutputModel extends TripInputModel {
  id: number;
}
