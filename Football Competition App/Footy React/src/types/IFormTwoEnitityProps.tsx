export interface IFormTwoEntityProps<TEntity, DEntity> {
    firstValues: TEntity;
    secondValues: DEntity;
    handleChange: (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => void;
}

