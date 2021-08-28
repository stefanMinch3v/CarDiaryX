import { AddressModel } from './address.model';

export interface TripModel {
  departureDate: string;
  arrivalDate: string;
  departureAddress: AddressModel;
  arrivalAddress: AddressModel;
  registrationNumber: string,
  distance?: number;
  cost?: number;
  note?: string;
}
