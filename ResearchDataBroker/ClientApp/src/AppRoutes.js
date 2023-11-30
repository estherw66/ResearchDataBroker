import { Home } from "./components/Home";
import Test from "./components/Test";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/test-api',
    element: <Test />
  }
];

export default AppRoutes;
