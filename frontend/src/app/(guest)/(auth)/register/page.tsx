import Link from "next/link";

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { RegisterForm } from "@/features/auth";
import { routes } from "@/lib/routes";

export default function Register() {
  return (
    <main className="container mx-auto max-w-xl">
      <Card>
        <CardHeader>
          <CardTitle>Create an account</CardTitle>
          <CardDescription>
            Please enter your credentials to create an account.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <RegisterForm />
        </CardContent>
        <CardFooter>
          <p className="text-muted-foreground text-sm">
            Already have an account?{" "}
            <Link
              href={routes.login}
              className="font-medium underline-offset-4 hover:underline"
            >
              Log in
            </Link>
          </p>
        </CardFooter>
      </Card>
    </main>
  );
}
