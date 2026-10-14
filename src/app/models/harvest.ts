export interface Harvest {
  id: string;
  variety: string;
  harvestedBy: string;
  harvestDate: string;
  notes: string;
}

export type NewHarvest = Omit<Harvest, 'id'>;
