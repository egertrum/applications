export interface IFormProps<TEntity> {
    values: TEntity;
    handleChange: (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => void;
}

