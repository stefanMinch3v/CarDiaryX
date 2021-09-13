import { AddressModel } from './address.model';

export interface TripInputModel {
  departureDate: string;
  arrivalDate: string;
  departureAddress: AddressModel;
  arrivalAddress: AddressModel;
  registrationNumber: string,
  distance?: number;
  cost?: number;
  note?: string;
}
