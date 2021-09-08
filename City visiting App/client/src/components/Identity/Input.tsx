import React from 'react';
import { TextField, Grid } from '@material-ui/core';

const Input = ({ name, value, handleChange, label, half, autoFocus, type }: any) => (
  <Grid item xs={12} sm={half ? 6 : 12}>
    <TextField
      name={name}
      value={value}
      onChange={handleChange}
      variant="outlined"
      required
      fullWidth
      label={label}
      autoFocus={autoFocus}
      type={type}
    />
  </Grid>
);

export default Input;