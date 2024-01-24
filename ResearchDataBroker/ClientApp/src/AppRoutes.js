import { Home } from "./pages/Home";
import Test from "./pages/Test";
import Error from "./pages/error";
import SearchResults from "./pages/SearchResults";
// import * as path from "path";
import AddDataset from "./pages/AddDataset";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/search/:item',
    element: <SearchResults />
  },
  {
    path: '/*',
    element: <Error />
  },
  {
    path: '/add-dataset',
    element: <AddDataset />
  },
];

export default AppRoutes;
