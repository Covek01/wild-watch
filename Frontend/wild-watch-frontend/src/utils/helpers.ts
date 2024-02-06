import { User } from "../models/User"

export const lsGetToken: () => string | null = () => {
  const lstoken = localStorage.getItem("access-token");
  return lstoken;
};

export const lsSetToken = (token: string) =>
  localStorage.setItem("access-token", token);

export const lsRemoveToken = () => localStorage.removeItem("access-token");

export const lsGetUser = () => JSON.parse(localStorage.getItem("user") || "{}");

export const lsSetUser = (user: User) =>
  localStorage.setItem("user", JSON.stringify(user));

export const lsRemoveUser = () => localStorage.removeItem("user");
