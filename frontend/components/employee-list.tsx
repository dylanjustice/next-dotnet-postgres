import { Button, Table, TableCell, TableRow } from "@mui/material";
import React, { useState } from "react";
import useSWR, { BareFetcher, Fetcher } from "swr";
import { Employee } from "../interfaces/models/employee";
import { Result } from "../interfaces/models/result";

// -----------------------------------------------------------------------------------------
// #region Interfaces
// -----------------------------------------------------------------------------------------

export interface EmployeeListProps {}

// #endregion Interfaces

// -----------------------------------------------------------------------------------------
// #region Component
// -----------------------------------------------------------------------------------------

const EmployeeList: React.FC<EmployeeListProps> = (
  props: EmployeeListProps
) => {
  const fetcher: Fetcher<Result<Array<Employee>>> = (
    input: RequestInfo,
    init?: RequestInit | undefined
  ) => fetch(input, init).then((res) => res.json());

  const [shouldFetch, setShouldFetch] = useState<boolean>(true);
  const { data, error } = useSWR<Result<Array<Employee>>, any>(
    `/api/v1/employees?fetch=${shouldFetch}`,
    fetcher
  );

  const handleClick = () => {
      setShouldFetch(!shouldFetch);
      console.log(`setShouldFetch, value: ${shouldFetch}`);
  }

  if (error) {
    return <div>Failed to load</div>;
  }
  if (!data) {
    return <div>Loading...</div>;
  }
  const employees = data.resultObject;

  return (
    <div>
      <h2>Employees</h2>

          <Table>
        {employees.map((e: Employee) => {
          return (
              <TableRow key={e.ipAddress}>
              <TableCell>{e.email}</TableCell>
              <TableCell>{e.firstName}</TableCell>
              <TableCell>{e.lastName}</TableCell>
              <TableCell>{e.gender}</TableCell>
              <TableCell>{e.ipAddress}</TableCell>
              </TableRow>

          );
        })}

      </Table>
      <Button onClick={handleClick}>Refresh</Button>
    </div>
  );
};

// #endregion Component

// -----------------------------------------------------------------------------------------
// #region Exports
// -----------------------------------------------------------------------------------------

export default EmployeeList;

// #endregion Exports
